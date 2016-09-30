#import distutils.sysconfig
#newPath = distutils.sysconfig.get_python_lib() 
# Add a bin directory for dlls to the path:
newPath = __path__[0] + "\\bin"
import sys
sys.path.append(newPath)

#--- Numpy install

import os
import sys
import tempfile
import zipfile
from os.path import dirname, isdir, join

def unzip(zip_file, dir_path):
    """Unzip the zip_file into the directory dir_path."""
    z = zipfile.ZipFile(zip_file)
    for name in z.namelist():
        if name.endswith('/'):
            continue
        path = join(dir_path, *name.split('/'))
        if not isdir(dirname(path)):
            os.makedirs(dirname(path))
        fo = open(path, 'wb')
        fo.write(z.read(name))
        fo.close()
    z.close()

def self_install():
    tmp_dir = tempfile.mkdtemp()
    egg_path = 'numpy-2.0.0-1.egg'
    egg_path2 = 'scipy-1.0.0-2.egg'
    unzip(egg_path, tmp_dir)
    unzip(egg_path2, tmp_dir)
    sys.path.insert(0, tmp_dir)
    import egginst
    print "Bootstrapping:", egg_path
    ei = egginst.EggInst(egg_path)
    ei.install()
    print "Bootstrapping:", egg_path2
    ei2 = egginst.EggInst(egg_path2)
    ei2.install()

#--- /Numpy install

try:
    import numpy
except Exception as ex:
    try:
        print 'attempting to install numpy for Iron Python...'
        sys.prefix = os.getcwd()  #base dir of app
        self_install()
        x = raw_input('IronLab must be restarted')
    except Exception as ex:
        print 'problem while intalling numpy for IP: ', str(ex)


import ironplot_windows
import ironplot_functions
import ironplot_mscharts

from ironplot_windows import dispatch
from ironplot_functions import plot, image, plot3d, xlabel, ylabel, title, equalaxes, window \
    , currentplot, currplot, tab, hold, subplot \
    , MarkersType, Position \
    , Plot2D, Plot2DCurve, FalseColourImage, QuickStrokeDash, Plot3D, XAxis, YAxis, XAxisPosition, YAxisPosition \
    , MSChartHost, FormatOverrides
from ironplot_mscharts import radial

import clr
from System.Windows import Thickness, Visibility, FontStyles, FontWeights
from System.Windows.Controls import Orientation
from System.Windows.Media import Brushes


