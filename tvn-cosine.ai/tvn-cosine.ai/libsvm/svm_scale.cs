using System;
using System.IO;
using tvn.cosine.text;
using tvn.cosine.text.api;

namespace tvn.cosine.ai.libsvm
{
    class svm_scale
    {
        private string line = null;
        private double lower = -1.0;
        private double upper = 1.0;
        private double y_lower;
        private double y_upper;
        private bool y_scaling = false;
        private double[] feature_max;
        private double[] feature_min;
        private double y_max = -double.MaxValue;
        private double y_min = double.MaxValue;
        private int max_index;
        private long num_nonzeros = 0;
        private long new_num_nonzeros = 0;

        private static void exit_with_help()
        {
            System.Console.WriteLine(
             "Usage: svm-scale [options] data_filename\n"
            + "options:\n"
            + "-l lower : x scaling lower limit (default -1)\n"
            + "-u upper : x scaling upper limit (default +1)\n"
            + "-y y_lower y_upper : y scaling limits (default: no y scaling)\n"
            + "-s save_filename : save scaling parameters to save_filename\n"
            + "-r restore_filename : restore scaling parameters from restore_filename\n"
            );
            System.Environment.Exit(1);
        }

        private StreamReader rewind(StreamReader fp, string filename)
        {
            fp.Close();
            return new StreamReader(filename);
        }

        private void output_target(double value)
        {
            if (y_scaling)
            {
                if (value == y_min)
                    value = y_lower;
                else if (value == y_max)
                    value = y_upper;
                else
                    value = y_lower + (y_upper - y_lower) *
                    (value - y_min) / (y_max - y_min);
            }

            System.Console.WriteLine(value + " ");
        }

        private void output(int index, double value)
        {
            /* skip single-valued attribute */
            if (feature_max[index] == feature_min[index])
                return;

            if (value == feature_min[index])
                value = lower;
            else if (value == feature_max[index])
                value = upper;
            else
                value = lower + (upper - lower) *
                    (value - feature_min[index]) /
                    (feature_max[index] - feature_min[index]);

            if (value != 0)
            {
                System.Console.WriteLine(index + ":" + value + " ");
                new_num_nonzeros++;
            }
        }

        private string readline(StreamReader fp)
        {
            line = fp.ReadLine();
            return line;
        }

        private void run(params string[] argv)
        {
            int i, index;
            StreamReader fp = null, fp_restore = null;
            string save_filename = null;
            string restore_filename = null;
            string data_filename = null;


            for (i = 0; i < argv.Length; i++)

            {
                if (argv[i][0] != '-') break;
                ++i;
                switch (argv[i - 1][1])
                {
                    case 'l': lower = TextFactory.ParseDouble(argv[i]); break;
                    case 'u': upper = TextFactory.ParseDouble(argv[i]); break;
                    case 'y':
                        y_lower = TextFactory.ParseDouble(argv[i]);
                        ++i;
                        y_upper = TextFactory.ParseDouble(argv[i]);
                        y_scaling = true;
                        break;
                    case 's': save_filename = argv[i]; break;
                    case 'r': restore_filename = argv[i]; break;
                    default:
                        System.Console.WriteLine("unknown option");
                        exit_with_help();
                        break;
                }
            }

            if (!(upper > lower) || (y_scaling && !(y_upper > y_lower)))

            {
                System.Console.WriteLine("inconsistent lower/upper specification");
                System.Environment.Exit(1);
            }
            if (restore_filename != null && save_filename != null)

            {
                System.Console.WriteLine("cannot use -r and -s simultaneously");
                System.Environment.Exit(1);
            }

            if (argv.Length != i + 1)

                exit_with_help();

            data_filename = argv[i];
            try
            {
                fp = new StreamReader(data_filename);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("can't open file " + data_filename);
                System.Environment.Exit(1);
            }

            /* assumption: min index of attributes is 1 */
            /* pass 1: find out max index of attributes */
            max_index = 0;

            if (restore_filename != null)

            {
                int idx, c;

                try
                {
                    fp_restore = new StreamReader(restore_filename);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("can't open file " + restore_filename);
                    System.Environment.Exit(1);
                }
                if ((c = fp_restore.Read()) == 'y')
                {
                    fp_restore.ReadLine();
                    fp_restore.ReadLine();
                    fp_restore.ReadLine();
                }
                fp_restore.ReadLine();
                fp_restore.ReadLine();


                string restore_line = null;
                while ((restore_line = fp_restore.ReadLine()) != null)
                {
                    var st2 = restore_line.Split(new[] { ' ', '\t', '\n', '\r', '\f', ':' });
                    idx = TextFactory.ParseInt(st2[0]);
                    max_index = Math.Max(max_index, idx);
                }
                fp_restore = rewind(fp_restore, restore_filename);
            }

            while (readline(fp) != null)
            {
                int counter = 1;
                var st = line.Split(new[] { ' ', '\t', '\n', '\r', '\f', ':' });

                while (st.Length > counter)
                {
                    index = TextFactory.ParseInt(st[counter++]);
                    max_index = Math.Max(max_index, index);
                    ++counter;
                    ++num_nonzeros;
                }
            }

            try
            {
                feature_max = new double[(max_index + 1)];
                feature_min = new double[(max_index + 1)];
            }
            catch (Exception e)
            {
                System.Console.WriteLine("can't allocate enough memory");
                System.Environment.Exit(1);
            }

            for (i = 0; i <= max_index; i++)

            {
                feature_max[i] = -double.MaxValue;
                feature_min[i] = double.MaxValue;
            }

            fp = rewind(fp, data_filename);

            /* pass 2: find out min/max value */
            while (readline(fp) != null)

            {
                int next_index = 1;
                double target;
                double value;
                int counter = 0;
                var st = line.Split(new[] { ' ', '\t', '\n', '\r', '\f', ':' });
                target = TextFactory.ParseDouble(st[counter++]);
                y_max = Math.Max(y_max, target);
                y_min = Math.Min(y_min, target);

                while (st.Length > counter)
                {
                    index = TextFactory.ParseInt(st[counter++]);
                    value = TextFactory.ParseDouble(st[counter++]);

                    for (i = next_index; i < index; i++)
                    {
                        feature_max[i] = Math.Max(feature_max[i], 0);
                        feature_min[i] = Math.Min(feature_min[i], 0);
                    }

                    feature_max[index] = Math.Max(feature_max[index], value);
                    feature_min[index] = Math.Min(feature_min[index], value);
                    next_index = index + 1;
                }

                for (i = next_index; i <= max_index; i++)
                {
                    feature_max[i] = Math.Max(feature_max[i], 0);
                    feature_min[i] = Math.Min(feature_min[i], 0);
                }
            }
            fp = rewind(fp, data_filename);

            /* pass 2.5: save/restore feature_min/feature_max */
            if (restore_filename != null)

            {
                // fp_restore rewinded in finding max_index 
                int idx, c;
                double fmin, fmax;
                if ((c = fp_restore.Read()) == 'y')
                {
                    fp_restore.ReadLine();      // pass the '\n' after 'y'
                    int counter = 0;
                    var st = fp_restore.ReadLine().Split(new[] { ' ', '\t', '\n', '\r', '\f', ':' });
                    y_lower = TextFactory.ParseDouble(st[counter++]);
                    y_upper = TextFactory.ParseDouble(st[counter++]);
                    st = fp_restore.ReadLine().Split(new[] { ' ', '\t', '\n', '\r', '\f', ':' });
                    y_min = TextFactory.ParseDouble(st[counter++]);
                    y_max = TextFactory.ParseDouble(st[counter++]);
                    y_scaling = true;
                }
                else
                    fp = rewind(fp, data_filename);

                if (fp_restore.Read() == 'x')
                {
                    fp_restore.ReadLine();      // pass the '\n' after 'x'
                    int counter = 0;
                    var st = fp_restore.ReadLine().Split(new[] { ' ', '\t', '\n', '\r', '\f', ':' });
                    lower = TextFactory.ParseDouble(st[counter++]);
                    upper = TextFactory.ParseDouble(st[counter++]);
                    string restore_line = null;
                    while ((restore_line = fp_restore.ReadLine()) != null)
                    {
                        int counter2 = 0;
                        var st2 = restore_line.Split(new[] { ' ', '\t', '\n', '\r', '\f', ':' });
                        idx = TextFactory.ParseInt(st2[counter2++]);
                        fmin = TextFactory.ParseDouble(st2[counter2++]);
                        fmax = TextFactory.ParseDouble(st2[counter2++]);
                        if (idx <= max_index)
                        {
                            feature_min[idx] = fmin;
                            feature_max[idx] = fmax;
                        }
                    }
                }
                fp_restore.Close();
            }

            if (save_filename != null)

            {
                StreamWriter fp_save = null;
                IStringBuilder sb = TextFactory.CreateStringBuilder();

                try
                {
                    fp_save = new StreamWriter(save_filename);
                }
                catch (IOException e)
                {
                    System.Console.WriteLine("can't open file " + save_filename);
                    System.Environment.Exit(1);
                }

                if (y_scaling)
                {
                    sb.Append("y\n");
                    sb.Append(string.Format("{0} {1}\n", y_lower.ToString("N16"), y_upper.ToString("N16")));
                    sb.Append(string.Format("{0} {1}\n", y_min.ToString("N16"), y_max.ToString("N16")));
                }
                sb.Append("x\n");
                sb.Append(string.Format("{0} {1}\n", lower.ToString("N16"), upper.ToString("N16")));
                for (i = 1; i <= max_index; i++)
                {
                    if (feature_min[i] != feature_max[i])
                        sb.Append(string.Format("{0} {1}\n", i, feature_min[i].ToString("N16"), feature_max[i].ToString("N16")));
                }
                fp_save.Write(sb.ToString());
                fp_save.Close();
            }

            /* pass 3: scale */
            while (readline(fp) != null)

            {
                int next_index = 1;
                double target;
                double value;
                int counter = 0;

                var st = line.Split(new[] { ' ', '\t', '\n', '\r', '\f', ':' });
                target = TextFactory.ParseDouble(st[counter++]);
                output_target(target);
                while (st.Length > counter)
                {
                    index = TextFactory.ParseInt(st[counter++]);
                    value = TextFactory.ParseDouble(st[counter++]);
                    for (i = next_index; i < index; i++)
                        output(i, 0);
                    output(index, value);
                    next_index = index + 1;
                }

                for (i = next_index; i <= max_index; i++)
                    output(i, 0);
                System.Console.WriteLine("\n");
            }
            if (new_num_nonzeros > num_nonzeros)
                System.Console.WriteLine(
                 "WARNING: original #nonzeros " + num_nonzeros + "\n"
                + "         new      #nonzeros " + new_num_nonzeros + "\n"
                + "Use -l 0 if many original feature values are zeros\n");

            fp.Close();
        }

        public static void Main(params string[] argv)
        {
            svm_scale s = new svm_scale();
            s.run(argv);
        }
    } 
}
